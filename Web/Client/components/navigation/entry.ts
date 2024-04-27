export interface NavigationEntry {
  key: string;
  name: string | React.ReactNode;
  href?: string;
  icon: React.ReactNode;
  children?: NavigationEntry[];
  seperate?: boolean;
}
