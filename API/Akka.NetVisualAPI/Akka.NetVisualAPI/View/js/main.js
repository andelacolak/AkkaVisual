 var nodes, edges, network, logData, graphData, timelineData, colors, groups, scale, keysList, listOfVectorClocks;

 $(function() {
  nodes = new vis.DataSet();
  edges = new vis.DataSet();
  //different colors for different projects
  //find a way to have infinite projects
  colors = ["#B0A1BA", "#A5B5BF", "#ABC8C7", "#B8E2C8", "#BFF0D4"];
  groups = [];
  scale = 1.0;
  keysList = [];
  listOfVectorClocks = [];
  DrawGraph();
  DrawTimeline();
  ConnectToServer();

  $( window ).unload(function() {
    $.connection.hub.stop();
  });
});

 function DrawGraph() {
  var options = GetOptions();
  var container = document.getElementById('mynetwork');
  // provide the data in the vis format
  graphData = {
    nodes: nodes,
    edges: edges
  }
  network = new vis.Network(container, graphData, options);
}

function DrawTimeline() {
  var timelineContainer = document.getElementById('mytimeline');
  timelineData = new vis.DataSet([]);
  var options = {
    height: "50%",
    editable: true,
    start: new Date(0,0,0,0,0,0,0),
    end: new Date(0,0,0,0,0,0,2)
  };
  var timeline = new vis.Timeline(timelineContainer, timelineData, options);
}

function AddDataToGraph(logData) {
  AddNode(logData.sender.path, GetGroup(logData.sender.path), logData.sender.type); 
  AddNode(logData.receiver.path, GetGroup(logData.receiver.path), logData.receiver.type);
  AddEdge(logData);
}

function AddDataToTimeline(logData) {
  var sender = logData.sender.path;
  var receiver = logData.receiver.path;
  timelineData.add({id : logData.id, content: "{0} sent <br>message {1}<br>to {2}".format(sender, logData.message.name, receiver), start: new Date(0,0,0,0,0,0,logData.index), className: 'green'});
}

function UpdateData(logData) {
  timelineData.update({id: logData.id, start: new Date(0,0,0,0,0,0,logData.index)});
  edges.update({id: logData.id, label: logData.index + " - " + logData.message.name});
}

String.prototype.format = function() {
  a = this;
  for (k in arguments) {
    a = a.replaceAll("{" + k + "}", arguments[k])
  }
  return a
}

String.prototype.replaceAll = function(search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

function ExtractName(fullName) {
  var fullNameArray = fullName.split("/");
  return fullNameArray.slice(3, fullNameArray.length).join("/");
}

function AddGroup(group) {
  if(!groups.includes(group)) {
    groups.push(group);
    AddGroupToLegend(group);
  }
}

function AddGroupToLegend(group) {
  $("#legend").append('<h6 class="legend-content"> <span class="badge badge-secondary" style=" background-color: ' + GetColor(group) + '"> </span>' + group + '</h1>');
}

function GetGroup(fullName) {
  var group = fullName.split("/")[2];
  AddGroup(group);
  return group;
}

//having troubles with dynamic group add
function GetColor(group) {
  return colors[groups.indexOf(group)];
}

function AddNode(node, group, type) {
  var shortNode = ExtractName(node);
  if(!graphData.nodes.getIds().includes(node)) {
    nodes.add({id: node, label: shortNode, color: GetColor(group), title: type});
  } 
}

function AddEdge(logData) {
  if(!graphData.edges.getIds().includes(logData.id)) {
    edges.add({id: logData.id, from: logData.sender.path, to: logData.receiver.path, label: logData.index + " - " + logData.message.name, title: FormatProps(logData.message.props)});
  }
}

function SetScale(id) {
  if(id == "plus") {
    scale+=0.25;
  } 
  else if (id == "minus") {
    if(scale > 0.5) { scale-=0.25; }
  }
}

function Zoom(id) {
  SetScale(id);
  var scaleOption = { 
    scale : scale,
    animation: {         
      duration: 800,                
      easingFunction: "easeInOutQuad" 
    }  
  };
  network.moveTo(scaleOption);
}

function RePlay() {
  EmptyGraph();
  DrawGraph();
  $.each( listOfVectorClocks, function( index, value ) {
    ScaledTimeout(index, value);
  });
}

function ScaledTimeout(i, value) {
  setTimeout(function() {
    AddDataToGraph(value);
  }, i * 1000);
}

function EmptyGraph() {
  nodes = new vis.DataSet();
  edges = new vis.DataSet();
}

function HappenedBefore(newClock, oldClock) {
  var boolean = true;
  $.each( newClock.clock, function( key, value ) {
    if(value > oldClock.clock[key]) { 
      boolean = false;
      return boolean;
    }
  });
  return boolean;
}

function HappenedAfter(newClock, oldClock) {
  var boolean = true;
  $.each( newClock.clock, function( key, value ) {
    //newClock.clock[key] = parseInt(newClock.clock[key]);
    if(value < oldClock.clock[key]) { 
      boolean = false;
      return boolean;
    }
  });
  return boolean;
}

function Concurrent(newClock, oldClock) {
  return !HappenedBefore(newClock, oldClock) && !HappenedAfter(oldClock, newClock);
}

function FillMissingKeys(data) {
  keysList.forEach(function(value) {
    if(!(value in data.clock)) {
      data.clock[value] =  0;
    }
  });
}

function AddToGlobalKeyList(data) {
  for(var key in data.clock) {
    data.clock[key] = parseInt(data.clock[key]);
    if(keysList.indexOf(key) == -1) {
      keysList.push(key);
    }
  }
}

function MoveBiggerVectorClocks(index) {
  for (var i = index; i < listOfVectorClocks.length; ++i) {
    listOfVectorClocks[i].index +=1;
    UpdateData(listOfVectorClocks[i]);
  }
}

function AddIndexes(data) {
  FillMissingKeys(data);
  AddToGlobalKeyList(data);
  if(listOfVectorClocks.length == 0) {
    data["index"] = 0;
    listOfVectorClocks.splice(data["index"], 0, data);
  } 
  else {
    var happenedBefore = false;
    for (var i = listOfVectorClocks.length - 1; i >= 0 ; i--) {
      var item = listOfVectorClocks[i];
      FillMissingKeys(item);

      if(HappenedAfter(data, item)) {
        data["index"] = item["index"] + 1;
        listOfVectorClocks.splice(i + 1, 0, data);
        if (happenedBefore) { MoveBiggerVectorClocks(i + 2); }
        break;
      }
      else if(Concurrent(data, item)) {
        data["index"] = item["index"];
        listOfVectorClocks.splice(i, 0, data);
        break;
      } 
      else {
        happenedBefore = true;

        if(i == 0) {
          data["index"] = 0;
          listOfVectorClocks.splice(0, 0, data);
          MoveBiggerVectorClocks(1);
        }
      }
    }
  }
  AddDataToGraph(data);
  AddDataToTimeline(data);
  //insert to list
}

function AddID(data) {
  data["id"] = new Date().getTime() * 10000 + 621355968000000000;
}

function FormatProps(props) {
  var result = "";
  Object.keys(props).forEach(function(k){
      result += "<p>{0} - {1}</p>".format(k, props[k]);
  });
  return result;
}

function  HideLoading() {
  $("#modal").fadeOut("slow");
  $(".jumbotron").removeClass("blur").addClass("unblur");
}

function ConnectToServer() {
  //Stored reference to the hub.
  var visualHub = $.connection.visualHub;
  //Initialize the connection.
  $.connection.hub.start().done(function () {
    console.log("success");
  });

  visualHub.client.broadcastMessage = function (data) {
    //HideLoading();
    console.log(data);
    AddID(data);
    AddIndexes(data);
  };

  $( ".zoom-btn" ).click(function(){Zoom(this.id)});
  $( "#redo" ).click(function(){RePlay()});
}
