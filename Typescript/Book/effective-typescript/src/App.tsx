import './App.css';
import {BrowserRouter, Routes, Route} from "react-router-dom";
import Item10 from './Demos/Item10';
import Item11 from './Demos/Item11';
import Layout from './Layout';
import DemosIndex from './Demos/DemosIndex';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout/>}>
          <Route path="demos" element={<DemosIndex/>}/>
          <Route path="/item10" element={<Item10/>}/>
          <Route path="/item11" index element={<Item11/>}/>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
