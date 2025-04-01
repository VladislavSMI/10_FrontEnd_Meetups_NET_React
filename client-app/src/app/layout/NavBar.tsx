import { observer } from "mobx-react-lite";
import { Link, NavLink } from "react-router-dom";
import { Container, Menu, Image, Dropdown } from "semantic-ui-react";
import { useStore } from "../stores/store";

export default observer(function NavBar() {
  const {
    userStore: { user, logout },
  } = useStore();
  return (
    <Menu inverted stackable fixed="top">
      <Container>
        <Menu.Item as={NavLink} to="/" exact header>
          <img
            src="/assets/logo.png"
            alt="logo"
            style={{ marginRight: "10px" }}
          />
          WebDev
        </Menu.Item>
        <Menu.Item as={NavLink} to="/activities" name="MeetUps" header />
        <Menu.Item
          as={NavLink}
          to="/createActivity"
          name="Create MeetUp"
          header
        ></Menu.Item>
        <Menu.Item position="right">
          <Image
            src={user?.image || "/assets/user.png"}
            avatar
            spaced="right"
          />
          <Dropdown
            inline
            text={user?.displayName}
            style={{ marginLeft: "1rem" }}
          >
            <Dropdown.Menu style={{ marginTop: "1rem", marginLeft: "-3.5rem" }}>
              <Dropdown.Item
                as={Link}
                to={`/profiles/${user?.userName}`}
                text="My Profile"
                icon="user"
              />
              <Dropdown.Item onClick={logout} text="Logout" icon="power" />
            </Dropdown.Menu>
          </Dropdown>
        </Menu.Item>
      </Container>
    </Menu>
  );
});
