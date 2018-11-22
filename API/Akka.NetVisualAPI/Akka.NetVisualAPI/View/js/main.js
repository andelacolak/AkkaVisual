 var nodes, edges, network, logData, graphData, timelineData, colors, groups, scale, keysList, listOfVectorClocks;

 $(function() {
  InitializeVariables();
  DrawGraph();
  DrawTimeline();
  ConnectToServer();

  $( window ).unload(function() {
    $.connection.hub.stop();
  });
});

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

function InitializeVariables() {
  nodes = new vis.DataSet();
  edges = new vis.DataSet();
  colors = ["#695540", "#772833", "#4C2646", "#444444"];
  groups = [];
  scale = 1.0;
  keysList = [];
  listOfVectorClocks = [];
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

function AddID(data) {
  data["id"] = new Date().getTime() * 10000 + 621355968000000000;
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
  $('[data-toggle="tooltip"]').tooltip()
}
