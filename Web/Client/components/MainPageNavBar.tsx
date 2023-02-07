import axios from "axios";
import { useCallback, useEffect, useRef, useState } from "react";
import { Container, Nav, Navbar } from "react-bootstrap";
import styles from "styles/NavBar.module.css";
import useIsMounted from "./misc/useIsMounted";

interface IMainPageProps {
  currentPath: string;
}

const MainPageNavBar = (props: IMainPageProps) => {
  const { currentPath } = props;

  const isMounted = useIsMounted();

  var [isLoggedIn, setLoggedIn] = useState<boolean>(false);

  useEffect(() => {
    if (document.cookie.includes(".AspNetCore.Cookies")) {
      setLoggedIn(true);
    }
  }, [isMounted]);

  return (
    <Navbar
      bg="dark"
      variant="dark"
      expand="lg"
      className={styles.navbar}
      sticky="top"
    >
      <Container>
        <Navbar.Brand href="/">Clive Bot</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link href="/" active={currentPath == "/"}>
              Home
            </Nav.Link>
            <Nav.Link href="/commands" active={currentPath == "/commands"}>
              Commands
            </Nav.Link>
          </Nav>
          <Nav className="mr-auto">
            <Nav.Link href={!isLoggedIn ? "/signin" : "/dashboard"}>
              {!isLoggedIn ? "" : "Dashboard"}
            </Nav.Link>
            <Nav.Link href={!isLoggedIn ? "/signin" : "/signout"}>
              {!isLoggedIn ? "Login" : "Logout"}
            </Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default MainPageNavBar;
