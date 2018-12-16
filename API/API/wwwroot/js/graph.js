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
  if(shortNode.length > 20) {shortNode = NewLine(shortNode)}
  if(!graphData.nodes.getIds().includes(node)) {
    nodes.add({id: node, label: shortNode, color: GetColor(group), title: type, font: {color: "white"}});
  } 
}

function AddEdge(logData) {
  if(!graphData.edges.getIds().includes(logData.id)) {
    edges.add({id: logData.id, from: logData.sender.path, to: logData.receiver.path, label: logData.index + " - " + logData.message.name, title: FormatProps(logData.message.props)});
  }
}

function EmptyGraph() {
  nodes = new vis.DataSet();
  edges = new vis.DataSet();
}

function NewLine(str) {
  return str.slice(0, Math.round(str.length/2)) + "- \n" + str.slice(Math.round(str.length/2), str.length);
}

function FormatProps(props) {
  var result = "";
  Object.keys(props).forEach(function(k){
      result += "<p>{0} - {1}</p>".format(k, props[k]);
  });
  return result;
}

