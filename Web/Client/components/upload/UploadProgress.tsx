import type { AxiosProgressEvent } from "axios";
import { humanFileSize, millisecondsToStr } from "../misc/fileSize";
import { Progress } from "../ui/progress";

export interface IUploadProgressProps {
  progress: AxiosProgressEvent | null;
}

export const UploadProgress = (props: IUploadProgressProps) => {
  const { progress } = props;

  return (
    <div className="mb-2">
      <Progress
        value={progress?.progress ?? 0}
        title={`${progress?.progress ?? 0}%`}
      />
      <div>
        <span className="text-center">
          {humanFileSize(progress?.bytes ?? 0)} /{" "}
          {humanFileSize(progress?.total ?? 0)} -{" "}
          {millisecondsToStr((progress?.estimated ?? 0) * 1000)}
        </span>
      </div>
    </div>
  );
};
