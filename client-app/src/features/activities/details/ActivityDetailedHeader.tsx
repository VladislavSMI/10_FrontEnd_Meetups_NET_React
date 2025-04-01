import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Button, Header, Item, Segment, Image, Label } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";
import { format } from "date-fns";
import { useStore } from "../../../app/stores/store";

const activityImageStyle = {
  filter: "brightness(50%)",
  minHeight: "15rem",
  objectFit: "cover",
};

const activityImageTextStyle = {
  position: "absolute",
  bottom: "5%",
  left: "5%",
  width: "100%",
  height: "auto",
  color: "white",
};

interface Props {
  activity: IActivity;
}

export default observer(function ActivityDetailedHeader({ activity }: Props) {
  const {
    activityStore: { updateAttendance, loading, cancelActivityToggle },
  } = useStore();

  return (
    <Segment.Group>
      <Segment inverted>
        {activity.isCancelled && (
          <Label
            style={{ position: "absolute", zIndex: 1000, left: -14, top: 20 }}
            ribbon
            color="red"
            content="Cancelled"
          />
        )}
        <Image
          src={`/assets/categoryImages/${activity.category}.jpg`}
          fluid
          style={activityImageStyle}
        />
        <Segment style={activityImageTextStyle} basic>
          <Item.Group>
            <Item>
              <Item.Content>
                <Header
                  size="large"
                  content={activity.title}
                  style={{ color: "white", width: "90%" }}
                />
                <p>{format(activity.date!, "dd MMM yyyy")}</p>
                <p>
                  Hosted by{" "}
                  <strong>
                    <Link to={`/profiles/${activity.host?.userName}`}>
                      {activity.host?.displayName}
                    </Link>
                  </strong>
                </p>
              </Item.Content>
            </Item>
          </Item.Group>
        </Segment>
      </Segment>
      <Segment clearing inverted>
        {activity.isHost ? (
          <>
            <Button
              color={activity.isCancelled ? "green" : "red"}
              floated="left"
              inverted
              content={
                activity.isCancelled
                  ? "Re-activate Activity"
                  : "Cancel Activity"
              }
              onClick={cancelActivityToggle}
              loading={loading}
            />
            <Button
              disabled={activity.isCancelled}
              as={Link}
              to={`/manage/${activity.id}`}
              color="yellow"
              inverted
              floated="right"
            >
              Manage Event
            </Button>
          </>
        ) : activity.isGoing ? (
          <Button loading={loading} onClick={updateAttendance}>
            Cancel attendance
          </Button>
        ) : (
          <Button
            disabled={activity.isCancelled}
            loading={loading}
            onClick={updateAttendance}
            color="yellow"
            inverted
          >
            Join Activity
          </Button>
        )}
      </Segment>
    </Segment.Group>
  );
});
