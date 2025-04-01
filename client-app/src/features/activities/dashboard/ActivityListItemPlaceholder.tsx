import React, { Fragment } from "react";
import { Segment, Button, Placeholder } from "semantic-ui-react";

export default function ActivityListItemPlaceholder() {
  return (
    <Fragment>
      <Placeholder inverted fluid style={{ marginTop: 25 }}>
        <Segment.Group>
          <Segment inverted style={{ minHeight: 110 }}>
            <Placeholder inverted>
              <Placeholder.Header image>
                <Placeholder.Line />
                <Placeholder.Line />
              </Placeholder.Header>
              <Placeholder.Paragraph>
                <Placeholder.Line />
              </Placeholder.Paragraph>
            </Placeholder>
          </Segment>
          <Segment inverted>
            <Placeholder inverted>
              <Placeholder.Line />
              <Placeholder.Line />
            </Placeholder>
          </Segment>
          <Segment inverted secondary style={{ minHeight: 70 }} />
          <Segment inverted clearing>
            <Button disabled color="yellow" floated="right" content="View" />
          </Segment>
        </Segment.Group>
      </Placeholder>
    </Fragment>
  );
}
