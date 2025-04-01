import { observer } from "mobx-react-lite";
import React from "react";
import { Segment, Grid, Icon } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";
import { format } from "date-fns";

interface Props {
  activity: IActivity;
}

export default observer(function ActivityDetailedInfo({ activity }: Props) {
  return (
    <Segment.Group>
      <Segment inverted>
        <Grid>
          <Grid.Column width={1}>
            <Icon size="large" color="yellow" name="info" />
          </Grid.Column>
          <Grid.Column width={11}>
            <span>{activity.description}</span>
          </Grid.Column>
        </Grid>
      </Segment>
      <Segment inverted>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="calendar" size="large" color="yellow" />
          </Grid.Column>
          <Grid.Column width={11}>
            <span>{format(activity.date!, "dd MMM yyyy h:mm aa")}</span>
          </Grid.Column>
        </Grid>
      </Segment>
      <Segment inverted>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="marker" size="large" color="yellow" />
          </Grid.Column>
          <Grid.Column width={11}>
            <span>
              {activity.venue}, {activity.city}
            </span>
          </Grid.Column>
        </Grid>
      </Segment>
    </Segment.Group>
  );
});
