import axios from "axios";
import { useState } from "react";
import { Accordion, Col, ListGroup, Row, Spinner } from "react-bootstrap";
import { ISkill, SkillSummon, summonList } from "../models/skill/SkillModel";
import Image from "next/image";
import { replaceCDN } from "../constants";
import {
	ErrorModal,
	ErrorModalInfo,
	getErrorInfo,
} from "../errors/ErrorHandler";

export const SkillOtherList = () => {
	const [skills, setSkills] = useState<ISkill[]>([]);

	const [error, setError] = useState<ErrorModalInfo | null>(null);

	const fetchOtherSkills = async () => {
		try {
			const params = new URLSearchParams({
				summon: SkillSummon[SkillSummon.None].toString(),
			});

			const res = await axios.get("/api/skill?" + params.toString());
			if (res.status == 200) {
				const newSkills = res.data as ISkill[];
				setSkills(newSkills);
			}
		} catch (err: any) {
			setError(getErrorInfo(err));
		}
	};

	return (
		<div>
			<Accordion>
				<Accordion.Item
					eventKey={"0"}
				>
					<Accordion.Header
						onClick={async () => {
							await fetchOtherSkills();
						}}
					>
						{SkillSummon.None}
					</Accordion.Header>
					<Accordion.Body>
						{skills == undefined ? (
							<Spinner animation="border" role="status">
								<span className="visually-hidden">Loading...</span>
							</Spinner>
						) : (
							<ListGroup variant="flush">
								{skills.map((s) => {
									return (
										<ListGroup.Item
											key={"skill-" + s.id}
											action
											href={"/dashboard/skills/" + s.id}
										>
											<Image
												alt=""
												src={replaceCDN(s.iconUrl ?? "")}
												width={30}
												height={30}
											/>
											<span style={{ marginLeft: "1em" }}>{s.name}</span>
										</ListGroup.Item>
									);
								})}
							</ListGroup>
						)}
					</Accordion.Body>
				</Accordion.Item>
			</Accordion>
			{error == null ? null : (
				<ErrorModal error={error} onHide={() => setError(null)} />
			)}
		</div>
	);
};
