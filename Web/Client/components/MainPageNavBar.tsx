import axios from "axios";
import { useEffect, useState } from "react";
import { Container, Nav, Navbar } from "react-bootstrap"
import styles from 'styles/NavBar.module.css';

interface IMainPageProps {
    currentPath: string;
}

const MainPageNavBar = (props: IMainPageProps) => {
    const { currentPath } = props;

    var [username, setUsername] = useState<string | null>(null);
  
    useEffect(() => {
        if(!document.cookie.includes(".AspNetCore.Cookies")) return;
        axios.get("/api/User/current").then((res) => {
            var jsonData = res.data;
            setUsername(jsonData["username"] + "#" + jsonData["discriminator"]);
        }).catch((_) => {
            setUsername(null);
        });
    }, []);

    return <Navbar bg="dark" variant="dark" expand="lg" className={styles.navbar} sticky="top">
    <Container>
      <Navbar.Brand href="/">Clive Bot</Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav" >
        <Nav className="me-auto">
          <Nav.Link href="/" active={currentPath == "/"}>Home</Nav.Link>
          <Nav.Link href="/commands" active={currentPath == "/commands"}>Commands</Nav.Link>
        </Nav>
        <Nav className="mr-auto">
          <Nav.Link href="/dashboard">{username == null ? "" : "Dashboard"}</Nav.Link>
          <Nav.Link href={username == null ? "/signin" : "/signout"}>
            {username == null ? "Login" : "Logout"}
          </Nav.Link>
        </Nav>
      </Navbar.Collapse>
    </Container>
  </Navbar>;
}

export default MainPageNavBar;