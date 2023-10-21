import { DemoItem } from "./DemoItem";

class Item10 implements DemoItem{
    Header = 'Item10 : ';
    Render = () => {
        return <h1>Item 10</h1>
    }

}
// const Item10 = () =>
// {
//     return <h1>Item 10</h1>
// }

export default Item10;