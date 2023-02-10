import axios from "axios";
import { Form, ProgressBar, Row } from "react-bootstrap";
import { humanFileSize, millisecondsToStr } from "../misc/fileSize";
import { AxiosProgressEvent } from "axios";

export interface IUploadProgressProps {
	progress: AxiosProgressEvent | null;
}

export const UploadProgress = (props: IUploadProgressProps) => {
	const { progress } = props;

	return (
	<div className="mb-2">
		<ProgressBar now={progress?.progress ?? 0} label={`${progress?.progress ?? 0}%`}/>
		<Row>
			<Form.Text style={{ textAlign: "center"}}>
				{humanFileSize(progress?.bytes ?? 0)} / {humanFileSize(progress?.total ?? 0)} - {millisecondsToStr((progress?.estimated ?? 0) * 1000)}
			</Form.Text>
		</Row>
	</div>
	);
}