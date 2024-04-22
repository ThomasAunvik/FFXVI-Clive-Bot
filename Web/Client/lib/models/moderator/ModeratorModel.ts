import type { IModeratorPermissions } from "@/lib/models/moderator/ModeratorPermissionsModel";

export interface IModerator {
  id: number;
  name: string;
  connectionSource: string;
  connectionId: string;

  permissions: IModeratorPermissions;
}
