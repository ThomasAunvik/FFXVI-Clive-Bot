import axios from "axios";
import Image from "next/image";
import { useState } from "react";
import { Accordion, Col, ListGroup, Row, Spinner } from "react-bootstrap";
import {
  type ISkill,
  SkillSummon,
  summonList,
} from "../../lib/models/skill/SkillModel";
import { replaceCDN } from "../constants";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
} from "../errors/ErrorHandler";

export const SkillSummonList = () => {
  const [allSkills, setSkills] = useState<
    { summon: SkillSummon; skills: ISkill[] }[]
  >([]);

  const [error, setError] = useState<ErrorModalInfo | null>(null);

  const fetchSummonSkills = async (summon: SkillSummon) => {
    try {
      const skillIndex = allSkills.findIndex((s) => s.summon == summon);
      if (skillIndex != -1) return;

      const params = new URLSearchParams({
        summon: SkillSummon[summon].toString(),
      });

      const res = await axios.get("/api/skill?" + params.toString());
      if (res.status == 200) {
        const newSkills = res.data as ISkill[];
        setSkills((val) => {
          return [...val, { summon: summon, skills: newSkills }];
        });
      }
    } catch (err: any) {
      setError(getErrorInfo(err));
    }
  };

  return (
    <div>
      <Accordion>
        {summonList.map((s, i) => {
          const skills = allSkills.find((sk) => sk.summon == s);

          return (
            <Accordion.Item
              eventKey={i.toString()}
              key={"summon-" + s.toString()}
            >
              <Accordion.Header
                onClick={async () => {
                  await fetchSummonSkills(s);
                }}
              >
                {s}
              </Accordion.Header>
              <Accordion.Body>
                {skills == undefined ? (
                  <Spinner animation="border" role="status">
                    <span className="visually-hidden">Loading...</span>
                  </Spinner>
                ) : (
                  <ListGroup variant="flush">
                    {skills.skills.map((s) => {
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
          );
        })}
      </Accordion>
      {error == null ? null : (
        <ErrorModal error={error} onHide={() => setError(null)} />
      )}
    </div>
  );
};
