import React, { Component } from 'react';
import {Grid, Header, Segment, Card, List, Divider} from 'semantic-ui-react';
import {Line} from 'react-chartjs-2';


class Graph extends Component {
  constructor(props){
    super(props)  
    var labels = []
    for(var i = 0; i < this.props.data.smoothened_list.length; i++){
      labels.push("Week " +(i+1))
    }
    
    this.state = {data: []}
  }

  componentWillMount(){
    var labels = []
    for(var i = 0; i < this.props.data.smoothened_list.length; i++){
      labels.push("Week " +(i+1))
    }
    this.setState({data:
      {
        labels:labels,
        datasets: [
          {
            label:"Original Data",
            fill:false,
            lineTension: 0.1,
            borderColor: 'rgba(0,0,0,87)',
            data:this.props.data.original_list
          },
          {
            label:"With Forecast",
            fill:false,
            lineTension: 0.1,
            borderColor: 'rgba(254,0,0,1)',
            data:this.props.data.smoothened_list
          }
        ]
    }
    })
  }
  
  render() {
    console.log(this.props.data)
    return (
      <Grid.Column style={{textAlign:'center'}}>
        <Segment>
        <Header as='h1' color='grey'>{this.props.title}</Header>
        <Divider/>
          <List>
            <List.Item><Header as='h3' color='grey'>(α) Smoothing Factor => {this.props.data.smoothening_factor.toFixed(2)} </Header></List.Item>
            <List.Item>{this.props.data.beta_factor > 0 ? <Header as='h3' color='grey'>(β) Trend =>  {this.props.data.beta_factor.toFixed(2)} </Header> : null}</List.Item>
            <List.Item><Header as='h3' color='grey'>SSE => {this.props.data.sse}</Header></List.Item>
          </List>
        <Line data={this.state.data} />
        </Segment>
      </Grid.Column>
    );
  }
}

export default Graph;

