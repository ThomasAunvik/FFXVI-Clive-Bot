import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import axios from "axios";
import { useEffect, useState } from "react";
import { Accordion, Button, ButtonGroup, Col, Collapse, Container, ListGroup, ListGroupItem, Row, Spinner } from "react-bootstrap";
import { ErrorModal, ErrorModalInfo, getErrorInfo } from "../errors/ErrorHandler";
import { ISkillLanguage } from "../models/skill/SkillLanguageModel";
import { SkillLanguageForm } from "./SkillLanguageForm";

export interface ISkillLanguageListProps {
	skillId: string;
}

export const SkillLanguageList = (
	props: ISkillLanguageListProps
) => {
	const { skillId } = props;

	const [error, setError] = useState<ErrorModalInfo | null>(null);
	const [collapseNew, setCollapseNew] = useState(false);

	const [languages, setLanguages] = useState<ISkillLanguage[] | null>(
		null
	);

	const fetchLanguages = async () => {
		const res = await axios.get(`/api/skill/${skillId}/languages`);
		if(res.status != 200) return;

		setLanguages(res.data as ISkillLanguage[]);
	}

	useEffect(() => {
		try {
			fetchLanguages();
		} catch(err: any) {
			setError(getErrorInfo(err));
		}
	}, [fetchLanguages]);

	return <div>
		<h3>Languages:</h3>
		{languages == null ? 
			<Spinner animation="border" role="status">
				<span className="visually-hidden">Loading...</span>
			</Spinner> :
		<div>
			<ButtonGroup className="mb-3">
				<Button
					onClick={() => setCollapseNew(true)}
				>
					<FontAwesomeIcon icon={faAdd} width={20} />
				</Button>
				<Button 
					variant="secondary"
					disabled={!collapseNew}
					onClick={() => setCollapseNew(false)}
				>
					Cancel
				</Button>
			</ButtonGroup>
			<Collapse in={collapseNew}>
				<div>
					<SkillLanguageForm
						skillId={skillId}
						onUpdate={(langs) => {
							setLanguages(langs);
							setCollapseNew(false);
						}}
					/>
				</div>
			</Collapse>
			<Accordion>
				<div>
					{languages.map(l => {
						return <Accordion.Item 
							eventKey={`locale-${l.locale}`}
							key={`locale-${l.locale}`}
							title={`Locale: ${l.locale}`}
						>
							<Accordion.Header>
							<div>
								<p>
									{l.locale.toUpperCase()}: {l.name}
								</p>
								<p>
									Description: {l.description}
								</p>
							</div>
							</Accordion.Header>
							<Accordion.Body>
								<SkillLanguageForm
									skillId={skillId}
									language={l}
									onDelete={async () => {
										try {
											var res = await axios.delete(`/api/skill/${skillId}/languages/${l.locale}`);
											if(res.status == 200) {
												setLanguages(res.data as ISkillLanguage[]);
											}
										} catch(err: any) {
											setError(getErrorInfo(err));
										} 
									}}
									onUpdate={(langs) => {
										setLanguages(langs);
									}}
								/>
							</Accordion.Body>
						</Accordion.Item>
					})}
				</div>
			</Accordion>
		</div>
		}
		{error == null ? null :
			<ErrorModal error={error} onHide={() => setError(null)} />
		}
	</div>
}