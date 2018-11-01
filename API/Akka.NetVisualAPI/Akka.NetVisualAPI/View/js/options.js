function GetOptions() {
  return {
    autoResize: true,
    height: '100%',
    width: '100%',
    configure: {
      enabled: false,
      filter: true,
      container: undefined,
      showButton: true
    },
    nodes:{
      borderWidth: 2,
      borderWidthSelected: 2,
      fixed: false,
      scaling: {
        label: true
      },
      shadow: true,
      shape: 'circle'
    },
    edges:{
      length: 200,
      arrows: 'to',
      scaling:{
        label: true,
      },
      shadow: true,
      smooth: true,
    }
  }
}