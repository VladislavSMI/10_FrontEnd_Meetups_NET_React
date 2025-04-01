import { observer } from "mobx-react-lite";
import React, { SyntheticEvent } from "react";
import { IProfile } from "../../app/models/profile";
import { Button, Reveal } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

interface Props {
  profile: IProfile;
}

export default observer(function FollowButton({ profile }: Props) {
  const { profileStore, userStore } = useStore();
  const { updateFollowing, loading } = profileStore;

  // Do not display button when we are reviewing our own profile
  if (userStore.user?.userName === profile.userName) return null;

  function handleFollow(e: SyntheticEvent, username: string) {
    e.preventDefault();

    profile.following
      ? updateFollowing(username, false)
      : updateFollowing(username, true);
  }

  return (
    <Reveal animated="move">
      <Reveal.Content visible style={{ width: "100%" }}>
        <Button
          fluid
          color="yellow"
          content={profile.following ? "Following" : "Not following"}
        />
      </Reveal.Content>
      <Reveal.Content hidden style={{ width: "100%" }}>
        <Button
          fluid
          basic
          color={profile.following ? "red" : "green"}
          content={profile.following ? "Unfollow" : "Follow"}
          loading={loading}
          onClick={(e) => handleFollow(e, profile.userName)}
        />
      </Reveal.Content>
    </Reveal>
  );
});
