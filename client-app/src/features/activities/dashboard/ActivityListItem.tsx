import React from "react";
import { Link } from "react-router-dom";
import { Button, Icon, Item, Label, Segment } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";
import { format } from "date-fns";
import ActivityListItemAttendee from "./ActivityListItemAttendee";

interface Props {
  activity: IActivity;
}

export default function ActivityListItem({ activity }: Props) {
  return (
    <Segment.Group>
      <Segment inverted>
        {activity.isCancelled && (
          <Label
            attached="top"
            color="red"
            content="Cancelled"
            style={{ textAlign: "center" }}
          />
        )}
        <Item.Group unstackable>
          <Item>
            <Item.Image
              style={{ marginBottom: 3 }}
              size="tiny"
              circular
              src={activity.host?.image || "/assets/user.png"}
            />
            <Item.Content>
              <Item.Header as={Link} to={`/activities/${activity.id}`}>
                {activity.title}
              </Item.Header>
              <Item.Description>
                {" "}
                Hosted by{" "}
                <Link to={`profiles/${activity.hostUsername}`}>
                  {activity.host?.displayName}
                </Link>
              </Item.Description>
              {activity.isHost && (
                <Item.Description>
                  <Label color="yellow">You are hosting this activity</Label>
                </Item.Description>
              )}
              {activity.isGoing && !activity.isHost && (
                <Item.Description>
                  <Label color="grey">You are going to this activity</Label>
                </Item.Description>
              )}
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment inverted>
        <span>
          <Icon name="clock" color="yellow" />
          {format(activity.date!, "dd MMM yyyy h:mm aa")}
          <span className="ml-2">
            <Icon name="marker" color="yellow" /> {activity.venue}
          </span>
        </span>
      </Segment>
      <Segment inverted secondary>
        <ActivityListItemAttendee attendees={activity.attendees!} />
      </Segment>
      <Segment inverted clearing>
        <Icon name="info" color="yellow" />
        {activity.description}
        <Button
          as={Link}
          to={`/activities/${activity.id}`}
          color="yellow"
          inverted
          floated="right"
          content="View"
        />
      </Segment>
    </Segment.Group>
  );
}
