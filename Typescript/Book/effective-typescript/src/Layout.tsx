import { Outlet, Link } from "react-router-dom";

const Layout = () => {
  return (
    <>
        <h1><Link to="demos">All Demos</Link></h1>
        <Outlet />
    </>
  )
};

export default Layout;