 var nodes, edges, logData, graphData, timelineData, i, colors, groups;
 $(function() {
                nodes = new vis.DataSet();
                edges = new vis.DataSet();
                //different colors for different projects
                //find a way to have infinite projects
                colors = ["#B0A1BA", "#A5B5BF", "#ABC8C7", "#B8E2C8", "#BFF0D4"];
                groups = [];
                DrawGraph();
                DrawTimeline();
                ConnectToServer();
                
            });

            function DrawGraph() {
              var options = GetOptions();
              var container = document.getElementById('mynetwork');
              // provide the data in the vis format
              graphData = {
                nodes: nodes,
                edges: edges
              }
              var network = new vis.Network(container, graphData, options);
            }

            function DrawTimeline() {
              var timelineContainer = document.getElementById('mytimeline');
              timelineData = new vis.DataSet([]);
              console.log(timelineData);
              var options = {
                editable: true,
                start: new Date(0,0,0,0,0,0,0),
                end: new Date(0,0,0,0,0,0,2)
              };
              var timeline = new vis.Timeline(timelineContainer, timelineData, options);
            }

            function AddDataToGraph(logData) {
              var sender = ExtractName(logData.Sender);
              var receiver = ExtractName(logData.Receiver);
              AddNode(sender, GetGroup(logData.Sender)); 
              AddNode(receiver, GetGroup(logData.Receiver));
              AddEdge(sender, receiver, logData.Message);
            }

            function AddDataToTimeline(logData) {
              var sender = logData.Sender;
              var receiver = logData.Receiver;
              timelineData.add({id : i, content: "{0} sent <br> message {1}<br> to {2}".format(sender, logData.Message, receiver), start: new Date(0,0,0,0,0,0,i), className: 'green'});
            }

            String.prototype.format = function() {
              a = this;
              for (k in arguments) {
                a = a.replace("{" + k + "}", arguments[k])
              }
              return a
            }

            function ExtractName(fullName) {
                var fullNameArray = fullName.split("/");
                return fullNameArray.slice(3, fullNameArray.length).join("/");
            }

            function AddGroup(group) {
              console.log("groups",groups);
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

            function AddNode(node, group) {
                if(!graphData.nodes.getIds().includes(node)) {
                    nodes.add({id: node, label: node, color: GetColor(group)});
                    console.log(nodes);
                } 
            }

            function AddEdge(sender, receiver, message) {
                edges.add({from: sender, to: receiver, label: message });
            }

            function ConnectToServer() {
                i = 0;
                //Stored reference to the hub.
                var visualHub = $.connection.visualHub;
                //Initialize the connection.
                $.connection.hub.start().done(function () {
                    console.log("success");
                    //call server
                    console.log(visualHub.server.send());
                });

                visualHub.client.broadcastMessage = function (data) {
                    AddDataToGraph(data);
                    AddDataToTimeline(data);
                    i++;
                };
            }