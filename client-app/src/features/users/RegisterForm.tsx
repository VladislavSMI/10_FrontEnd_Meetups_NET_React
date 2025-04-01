import React from "react";
import { Formik, Form, ErrorMessage } from "formik";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Button, Header, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import * as Yup from "yup";
import ValidationErrors from "../errors/ValidationErrors";

export default observer(function RegisterForm() {
  const { userStore } = useStore();

  return (
    <Segment inverted>
      <Header
        as="h2"
        content="Sign up to MeetUps"
        color="yellow"
        textAlign="center"
      />
      <Formik
        initialValues={{
          displayName: "",
          userName: "",
          email: "",
          password: "",
          error: null,
        }}
        onSubmit={(values, { setErrors }) =>
          userStore.register(values).catch((error) => setErrors({ error }))
        }
        validationSchema={Yup.object({
          displayName: Yup.string().required(),
          username: Yup.string().required(),
          email: Yup.string().required().email(),
          password: Yup.string().required(),
        })}
      >
        {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
          <Form
            className="ui form error"
            onSubmit={handleSubmit}
            autoComplete="off"
          >
            <MyTextInput name="displayName" placeholder="DisplayName" />
            <MyTextInput name="username" placeholder="Username" />
            <MyTextInput name="email" placeholder="Email" />
            <MyTextInput
              name="password"
              placeholder="Password"
              type="password"
            />
            <ErrorMessage
              name="error"
              render={() => <ValidationErrors errors={errors.error} />}
            />
            <Button
              disabled={!isValid || !dirty || isSubmitting}
              loading={isSubmitting}
              color="yellow"
              basic
              content="Register"
              type="submit"
              fluid
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
