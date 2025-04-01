import React, { useEffect, useState } from "react";
import { Button, Grid, Header } from "semantic-ui-react";
import PhotoWidgetCropper from "./PhotoWidgetCropper";
import PhotoWidgetDropZone from "./PhotoWidgetDropZone";

interface Props {
  loading: boolean;
  uploadPhoto: (file: Blob) => void;
}

export default function PhotoUploadWidget({ loading, uploadPhoto }: Props) {
  const [files, setFiles] = useState<any>([]);
  const [cropper, setCropper] = useState<Cropper>();

  function onCrop() {
    if (cropper) {
      cropper.getCroppedCanvas().toBlob((blob) => uploadPhoto(blob!));
    }
  }

  // Clean up our component for URL.createObjectURL => our goal is to dispose file.preview where we are using our memory to display image
  useEffect(() => {
    return () => {
      files.forEach((file: any) => URL.revokeObjectURL(file.preview));
    };
  }, [files]);

  return (
    <Grid stackable>
      <Grid.Column width={4} style={{ textAlign: "center" }}>
        <Header sub color="yellow" content="Step-1 - Add Photo" />
        <PhotoWidgetDropZone setFiles={setFiles} />
      </Grid.Column>
      <Grid.Column width={1} />
      <Grid.Column width={4} style={{ textAlign: "center" }}>
        <Header sub color="yellow" content="Step-2 - Resize image" />
        {files && files.length > 0 && (
          <PhotoWidgetCropper
            setCropper={setCropper}
            imagePreview={files[0].preview}
          />
        )}
      </Grid.Column>
      <Grid.Column width={1} />
      <Grid.Column width={4} style={{ textAlign: "center" }}>
        <Header sub color="yellow" content="Step-3 - Preview & Upload" />
        {files && files.length > 0 && (
          <div className="flex-center">
            <div
              className="img-preview"
              style={{
                minHeight: 200,
                minWidth: 200,
                overflow: "hidden",
              }}
            ></div>
            <Button.Group>
              <Button
                basic
                color="yellow"
                content="Upload"
                loading={loading}
                onClick={onCrop}
                icon="check"
              />
              <Button
                basic
                color="red"
                content="Close"
                disabled={loading}
                onClick={() => setFiles([])}
                icon="close"
              />
            </Button.Group>
          </div>
        )}
      </Grid.Column>
    </Grid>
  );
}
