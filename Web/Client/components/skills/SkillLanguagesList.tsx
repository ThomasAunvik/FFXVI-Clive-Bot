"use client";
import { faAdd } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import axios from "axios";
import { LoaderCircle, PlusIcon } from "lucide-react";
import { useCallback, useEffect, useState } from "react";
import type { ISkillLanguage } from "../../lib/models/skill/SkillLanguageModel";
import {
  ErrorModal,
  type ErrorModalInfo,
  getErrorInfo,
  toastError,
} from "../errors/ErrorHandler";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "../ui/accordion";
import { Button } from "../ui/button";
import { Collapsible, CollapsibleContent } from "../ui/collapsible";
import { SkillLanguageForm } from "./SkillLanguageForm";

export interface ISkillLanguageListProps {
  skillId: string;
}

export const SkillLanguageList = (props: ISkillLanguageListProps) => {
  const { skillId } = props;

  const [error, setError] = useState<ErrorModalInfo | null>(null);
  const [collapseNew, setCollapseNew] = useState(false);

  const [languages, setLanguages] = useState<ISkillLanguage[] | null>(null);

  const fetchLanguages = useCallback(async () => {
    const res = await axios.get(`/api/skill/${skillId}/languages`);
    if (res.status !== 200) return;

    setLanguages(res.data as ISkillLanguage[]);
  }, [skillId]);

  useEffect(() => {
    console.log("Is it not fetching?", languages);
    if (languages) return;

    try {
      fetchLanguages();
    } catch (err) {
      toastError(err);
    }
  }, [fetchLanguages, languages]);

  return (
    <div>
      <h3>Languages:</h3>
      {!languages ? (
        <div>
          <LoaderCircle className="animate-spin" />
          <span className="sr-only">Loading...</span>
        </div>
      ) : (
        <div>
          <div className="mb-3 flex flex-row gap-2">
            <Button onClick={() => setCollapseNew(true)}>
              <PlusIcon />
            </Button>
            <Button
              variant="secondary"
              disabled={!collapseNew}
              onClick={() => setCollapseNew(false)}
            >
              Cancel
            </Button>
          </div>
          <Collapsible open={collapseNew}>
            <CollapsibleContent>
              <SkillLanguageForm
                skillId={skillId}
                onUpdate={(langs) => {
                  setLanguages(langs);
                  setCollapseNew(false);
                }}
              />
            </CollapsibleContent>
          </Collapsible>
          <Accordion type="multiple">
            {languages.map((l) => {
              return (
                <AccordionItem
                  value={`locale-${l.locale}`}
                  key={`locale-${l.locale}`}
                  title={`Locale: ${l.locale}`}
                >
                  <AccordionTrigger>
                    <div>
                      <p>
                        {l.locale.toUpperCase()}: {l.name}
                      </p>
                      <p>Description: {l.description}</p>
                    </div>
                  </AccordionTrigger>
                  <AccordionContent>
                    <SkillLanguageForm
                      skillId={skillId}
                      language={l}
                      onDelete={async () => {
                        try {
                          const res = await axios.delete(
                            `/api/skill/${skillId}/languages/${l.locale}`,
                          );
                          if (res.status === 200) {
                            setLanguages(res.data as ISkillLanguage[]);
                          }
                        } catch (err) {
                          toastError(err);
                        }
                      }}
                      onUpdate={(langs) => {
                        setLanguages(langs);
                      }}
                    />
                  </AccordionContent>
                </AccordionItem>
              );
            })}
          </Accordion>
        </div>
      )}
      {error == null ? null : (
        <ErrorModal error={error} onHide={() => setError(null)} />
      )}
    </div>
  );
};
