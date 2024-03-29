import axios from "axios";
import { useEffect, useState } from "react";
import { Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import styles from "styles/NavBar.module.css";
import useIsMounted from "./misc/useIsMounted";

interface IDashboardNavBarProps {
  currentPath: string;
}

const redirectToLogin = () => {
  window.location.href = "/signin?redirect=" + window.location.pathname;
};

const DashboardNavBar = (props: IDashboardNavBarProps) => {
  const { currentPath } = props;

  var [username, setUsername] = useState<string | null>(null);

  const isMounted = useIsMounted();

  useEffect(() => {
    if (!document.cookie.includes(".AspNetCore.Cookies")) {
      redirectToLogin();
      return;
    }

    axios.get("/api/User/current").then((res) => {
      if (res.status != 200) {
        redirectToLogin();
      }

      if (isMounted()) {
        var jsonData = res.data;
        setUsername(jsonData["username"] + "#" + jsonData["discriminator"]);
      }
    });
  }, [isMounted]);

  return (
    <Navbar
      bg="dark"
      variant="dark"
      expand="lg"
      className={styles.navbar}
      sticky={"top"}
    >
      <Container>
        <Navbar.Brand href="/">Clive Bot</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link href="/dashboard" active={currentPath == "/dashboard"}>
              Dashboard
            </Nav.Link>
            <Nav.Link
              href="/dashboard/skills"
              active={currentPath == "/dashboard/skills"}
            >
              Skills
            </Nav.Link>
            <Nav.Link
              href="/dashboard/characters"
              active={currentPath == "/dashboard/characters"}
            >
              Characters
            </Nav.Link>
            <NavDropdown
              title="Settings"
              id="basic-nav-dropdown"
              active={currentPath.startsWith("/dashboard/settings")}
            >
              <NavDropdown.Item
                href="/dashboard/settings"
                active={currentPath == "/dashboard/settings"}
              >
                Open Settings
              </NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item
                href="/dashboard/settings/moderator"
                active={currentPath.startsWith("/dashboard/settings/moderator")}
              >
                Moderators
              </NavDropdown.Item>
            </NavDropdown>
          </Nav>
          <Nav className="mr-auto">
            <Nav.Link href={""}>{username ?? "Loading..."}</Nav.Link>
          </Nav>
          <Nav className="mr-auto">
            <Nav.Link href={"/signout"}>Signout</Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default DashboardNavBar;
