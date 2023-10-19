import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Container, Header, Segment, Image, Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

export default observer(function HomePage() {
  const { userStore, modalStore } = useStore();

  return (
    <Segment vertical className="masthead">
      <Container textAlign="center">
        <div className="flex-center">
          <Image
            size="massive"
            src="/assets/logo.png"
            alt="logo"
            style={{ width: 80 }}
          />
        </div>
        <Header as="h1" inverted>
          WebDev MeetUps
        </Header>
        {userStore.isLoggedIn ? (
          <>
            <Header as="h2" inverted content="Welcome" />
            <Button
              as={Link}
              to="/activities"
              size="huge"
              inverted
              color="yellow"
            >
              Go to MeetUps!
            </Button>
          </>
        ) : (
          <>
            <Button
              onClick={() => modalStore.openModal(<LoginForm />)}
              size="huge"
              inverted
              color="yellow"
            >
              Login
            </Button>
            <Button
              onClick={() => modalStore.openModal(<RegisterForm />)}
              size="huge"
              inverted
              color="yellow"
            >
              Register
            </Button>
          </>
        )}
      </Container>
    </Segment>
  );
});
