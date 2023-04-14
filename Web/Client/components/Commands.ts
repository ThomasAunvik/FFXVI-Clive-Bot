export interface ICommand {
  command: string;
  description: string;

  arguments: ICommandArgument[];
  permissions: ICommandPermission[];
}

export interface ICommandArgument {
  parameter: string;
  description: string;
  parameterType: string;

  nullable: boolean;
}

export interface ICommandCategory {
  name: string;
  description: string;

  commands: ICommand[];
}

export interface ICommandPermission {
  name: string;
}

export const commandCategories: ICommandCategory[] = [
  {
    name: "Skill",
    description: "Skill Category",
    commands: [
      {
        command: "skill",
        description: "List of skills per summon",
        arguments: [
          {
            parameter: "skillSummon",
            description: "List the skills of a summon",
            nullable: true,
            parameterType: "SkillSummon",
          },
          {
            parameter: "skill",
            description: "Select the target skill",
            nullable: true,
            parameterType: "AutoComplete(string)",
          },
          {
            parameter: "search",
            description: "Search for the target skill",
            nullable: true,
            parameterType: "string",
          },
          {
            parameter: "locale",
            description: "Select a Locale",
            nullable: true,
            parameterType: "LocaleOptions",
          },
        ],
        permissions: [],
      },
    ],
  },
];
