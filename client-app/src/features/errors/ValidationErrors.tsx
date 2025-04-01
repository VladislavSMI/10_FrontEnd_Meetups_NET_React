import React from "react";
import { Message } from "semantic-ui-react";
interface Props {
  //we will use type any as we need string[] or null or undefined
  errors: any;
}

export default function ValidationErrors({ errors }: Props) {
  return (
    <Message error>
      {errors && (
        <Message.List>
          {errors.map((err: any, i: any) => (
            <Message.Item key={i}>{err}</Message.Item>
          ))}
        </Message.List>
      )}
    </Message>
  );
}
