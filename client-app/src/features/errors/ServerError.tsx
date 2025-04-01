import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Header, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

export default observer(function ServerError() {
  const { commonStore } = useStore();

  console.log(`commonStore ${commonStore.error?.message}`);
  return (
    <Container>
      <Header inverted as="h1" content="Server Error" />
      <Header
        sub
        inverted
        as="h5"
        color="red"
        content={commonStore.error?.message}
      />
      {commonStore.error?.details && (
        <Segment inverted style={{ overflow: "scroll" }}>
          <Header as="h4" content="Stack trace" color="teal" />
          <code style={{ marginTop: "10px" }}>
            {" "}
            {commonStore.error.details}
          </code>
        </Segment>
      )}
    </Container>
  );
});
