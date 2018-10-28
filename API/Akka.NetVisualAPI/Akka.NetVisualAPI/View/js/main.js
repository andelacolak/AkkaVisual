 var nodes, edges, logData, graphData, timelineData, i;
 $(function() {
                nodes = new vis.DataSet();
                edges = new vis.DataSet();
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
                start: new Date(0,0,0,0,0,0,0),
                end: new Date(0,0,0,0,0,0,2)
              };
              var timeline = new vis.Timeline(timelineContainer, timelineData, options);
            }

            function AddDataToGraph(logData) {
              var sender = ExtractName(logData.Sender);
              var receiver = ExtractName(logData.Receiver);
              AddNode(sender); 
              AddNode(receiver);
              AddEdge(sender, receiver, logData.Message);
            }

            function AddDataToTimeline(logData) {
              var sender = ExtractName(logData.Sender);
              var receiver = ExtractName(logData.Receiver);
              timelineData.add({id : i, content: "{0} sent <br> message {1}<br> to {2}".format(sender, logData.Message, receiver), start: new Date(0,0,0,0,0,0,i)});
              console.log("timeline", timelineData);
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

            function AddNode(node) {
                if(!graphData.nodes.getIds().includes(node)) {
                    nodes.add({id: node, label: node});
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