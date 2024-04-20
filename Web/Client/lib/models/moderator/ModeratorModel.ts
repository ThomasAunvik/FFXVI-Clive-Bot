import type { IModeratorPermissions } from "./ModeratorPermissionsModel";

export interface IModerator {
  id: number;
  name: string;
  connectionSource: string;
  connectionId: string;

  permissions: IModeratorPermissions;
}
