import React, { Component } from 'react';
import Graph from './components/graph/index';
import {Grid} from 'semantic-ui-react';
import ses from '../../ses.json';
import des from '../../des.json';

import './App.css';



class App extends Component {
  constructor(props){
    super(props)
    this.state = {graphs: []}
  }

  componentWillMount(){
    this.setState({ses_data:ses, des_data:des},() => {
      var graphs = [{title: "SES", data:this.state.ses_data},{title: "DES", data:this.state.des_data}]
      this.setState({graphs})
    })
  }

  renderGraphs(){
    return this.state.graphs.map(graph => {
      return <Graph key={graph.title} data={graph.data} title={graph.title}/>})
  }

  render() {
    return (
      <Grid padded>
        <Grid.Row columns={2} > 
         {this.renderGraphs()}
        </Grid.Row> 
      </Grid>
    );
  }
}

export default App;

