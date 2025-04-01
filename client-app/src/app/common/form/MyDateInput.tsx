import React from "react";
import { useField } from "formik";
import { FormField, Label } from "semantic-ui-react";
import DatePicker, { ReactDatePickerProps } from "react-datepicker";

// Partial<ReactDatePickerProps> makes all props optional.
// We don’t need to explicitly pass required props (like onChange) because Formik handles them internally via helpers.

export default function MyDateInput(props: Partial<ReactDatePickerProps>) {
  const [field, meta, helpers] = useField(props.name!);
  return (
    // css for date picker we can target it styles with .react-datepicker-wrapper {}
    <FormField error={meta.touched && !!meta.error}>
      <DatePicker
        {...field}
        {...props}
        selected={(field.value && new Date(field.value)) || null}
        onChange={(value) => helpers.setValue(value)}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </FormField>
  );
}
