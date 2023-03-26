import { Link } from "react-router-dom";

export default function DemosIndex() {
    return (
        <nav>
        <ul>
          <li>
            <Link to="/Item10">Item10</Link>
          </li>
          <li>
            <Link to="/Item11">Item 11 : Recognize the Limits of Excess Property Checking</Link>
          </li>
        </ul>
      </nav>
    )
}