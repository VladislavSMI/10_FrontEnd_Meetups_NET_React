import { observer } from "mobx-react-lite";
import { Fragment } from "react";
import Calendar from "react-calendar";
import { Menu } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";

export default observer(function ActivityFilters() {
  const {
    activityStore: { predicate, setPredicate },
  } = useStore();

  return (
    <Fragment>
      <Menu
        inverted
        vertical
        size="large"
        style={{ width: "100%", marginTop: 25 }}
      >
        <Menu.Item header icon="filter" content="Filters" color="yellow" />
        <Menu.Item
          content="All Activities"
          active={predicate.has("all")}
          onClick={() => setPredicate("all", "true")}
        />
        <Menu.Item
          content="I'm going"
          active={predicate.has("isGoing")}
          onClick={() => setPredicate("isGoing", "true")}
        />
        <Menu.Item
          content="I'm hosting"
          active={predicate.has("isHost")}
          onClick={() => setPredicate("isHost", "true")}
        />
      </Menu>
      <Calendar
        onChange={(date: Date) => setPredicate("startDate", date as Date)}
        value={predicate.get("startDate") || new Date()}
      />
    </Fragment>
  );
});
