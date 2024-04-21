"use client";
import axios from "axios";
import { LoaderCircle } from "lucide-react";
import Image from "next/image";
import Link from "next/link";
import { useState } from "react";
import {
  type ISkill,
  SkillSummon,
  summonList,
} from "../../lib/models/skill/SkillModel";
import { replaceCDN } from "../constants";
import { toastError } from "../errors/ErrorHandler";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "../ui/accordion";
import { Skeleton } from "../ui/skeleton";

export const SkillSummonList = () => {
  const [allSkills, setSkills] = useState<
    { summon: SkillSummon; skills: ISkill[] }[]
  >([]);

  const fetchSummonSkills = async (summon: SkillSummon) => {
    try {
      const skillIndex = allSkills.findIndex((s) => s.summon === summon);
      if (skillIndex !== -1) return;

      const params = new URLSearchParams({
        summon: SkillSummon[summon].toString(),
      });

      const res = await axios.get(`/api/skill?${params.toString()}`);
      if (res.status === 200) {
        const newSkills = res.data as ISkill[];
        setSkills((val) => {
          return [...val, { summon: summon, skills: newSkills }];
        });
      }
    } catch (err) {
      toastError(err);
    }
  };

  return (
    <div>
      <Accordion type="multiple">
        {summonList.map((s, i) => {
          const skills = allSkills.find((sk) => sk.summon === s);

          return (
            <AccordionItem value={i.toString()} key={`summon-${s.toString()}`}>
              <AccordionTrigger
                onClick={async () => {
                  await fetchSummonSkills(s);
                }}
              >
                {s}
              </AccordionTrigger>
              <AccordionContent>
                {skills === undefined ? (
                  <div>
                    <LoaderCircle className="animate-spin" />
                    <span className="sr-only">Loading...</span>
                  </div>
                ) : (
                  <ul className="flex flex-col gap-4">
                    {skills.skills.map((s) => {
                      return (
                        <li key={`skill-${s.id}`}>
                          <Link
                            href={`/dashboard/skills/${s.id}`}
                            className="flex flex-row"
                          >
                            {s.iconUrl ? (
                              <Image
                                alt={`Skill ${s.name} Image`}
                                src={replaceCDN(s.iconUrl)}
                                width={40}
                                height={40}
                              />
                            ) : (
                              <Skeleton className="w-10 h-10" />
                            )}
                            <span className="ml-4 mt-2 text-base">
                              {s.name}
                            </span>
                          </Link>
                        </li>
                      );
                    })}
                  </ul>
                )}
              </AccordionContent>
            </AccordionItem>
          );
        })}
      </Accordion>
    </div>
  );
};
