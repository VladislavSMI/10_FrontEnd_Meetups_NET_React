import React, { SyntheticEvent, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Tab, Grid, Header, Card, Image, TabProps } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { IUserActivity } from "../../app/models/profile";
import { format } from "date-fns";
import { useStore } from "../../app/stores/store";
const panes = [
  { menuItem: "All Events", pane: { key: "" } },
  { menuItem: "Future Events", pane: { key: "future" } },
  { menuItem: "Past Events", pane: { key: "past" } },
  { menuItem: "Hosting", pane: { key: "hosting" } },
];

export default observer(function ProfileActivities() {
  const { profileStore } = useStore();
  const { loadUserActivities, profile, loadingActivities, userActivities } =
    profileStore;
  useEffect(() => {
    loadUserActivities(profile!.userName);
  }, [loadUserActivities, profile]);
  const handleTabChange = (e: SyntheticEvent, data: TabProps) => {
    loadUserActivities(
      profile!.userName,
      panes[data.activeIndex as number].pane.key
    );
  };
  return (
    <Tab.Pane inverted loading={loadingActivities}>
      <Grid>
        <Grid.Column width={16}>
          <Header
            inverted
            floated="left"
            icon="calendar"
            content={"Activities"}
          />
        </Grid.Column>
        <Grid.Column width={16}>
          <Tab
            panes={panes}
            menu={{
              inverted: true,
              secondary: true,
              pointing: true,
              stackable: true,
            }}
            onTabChange={(e, data) => handleTabChange(e, data)}
          />
          <br />
          <Card.Group centered>
            {userActivities.map((activity: IUserActivity) => (
              <Card
                as={Link}
                to={`/activities/${activity.id}`}
                key={activity.id}
              >
                <Image
                  src={`/assets/categoryImages/${activity.category}.jpg`}
                  style={{ minHeight: 100, objectFit: "cover" }}
                />
                <Card.Content>
                  <Card.Header textAlign="center">{activity.title}</Card.Header>
                  <Card.Meta textAlign="center">
                    <div>{format(new Date(activity.date), "do LLL")}</div>
                    <div>{format(new Date(activity.date), "h:mm a")}</div>
                  </Card.Meta>
                </Card.Content>
                .
              </Card>
            ))}
          </Card.Group>
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
});
