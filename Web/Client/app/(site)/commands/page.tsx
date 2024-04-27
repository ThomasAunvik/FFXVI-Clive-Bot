import Aux from "@/components/Auxillary";
import { commandCategories } from "@/components/Commands";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible";

const CommandsPage = () => {
  return (
    <div className="pt-20">
      <h1>Commands</h1>
      {commandCategories.map((category) => (
        <div key={`category-${category.name}`}>
          <h2>{category.name}</h2>
          <div>
            <div>
              <ul>
                <li>Slash Command</li>
                <li>Description</li>
                <li>Permissions</li>
              </ul>
            </div>
            <div>
              {category.commands.map((command) => (
                <Aux key={`command-${command.command}`}>
                  <Collapsible>
                    <div>
                      <CollapsibleTrigger>
                        <span>/{command.command}</span>
                        <span>{command.description}</span>
                        <span>
                          {command.permissions.map((p) => p.name).join(", ")}
                        </span>
                      </CollapsibleTrigger>
                    </div>
                    <div>
                      <CollapsibleContent>
                        <div>
                          <div>
                            <div>
                              <li>
                                <li>Arguments</li>
                                <li>Description</li>
                                <li>Parameter Type</li>
                                <li>Is Nullable</li>
                              </li>
                            </div>
                            <div>
                              {command.arguments.map((arg) => (
                                <ul
                                  key={`command-${command.command}-arg-${arg.parameter}`}
                                >
                                  <li>{arg.parameter}</li>
                                  <li>{arg.description}</li>
                                  <li>{arg.parameterType}</li>
                                  <li>{arg.nullable ? "Yes" : "No"}</li>
                                </ul>
                              ))}
                            </div>
                          </div>
                        </div>
                      </CollapsibleContent>
                    </div>
                  </Collapsible>
                </Aux>
              ))}
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default CommandsPage;
