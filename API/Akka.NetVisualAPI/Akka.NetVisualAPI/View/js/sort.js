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
        //ako ovi isprid njega ima isti id ka i on provjerit i njega
        //ako je on  zadnji stavit concurrent
        //ako isprid njega ima manji index od njega isto stavit concurrent
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

function MoveBiggerVectorClocks(index) {
  for (var i = index; i < listOfVectorClocks.length; ++i) {
    listOfVectorClocks[i].index +=1;
    UpdateData(listOfVectorClocks[i]);
  }
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