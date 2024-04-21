import DashboardNavBar from "@/components/navigation/DashboardNavBar";

interface LayoutProps {
  children: React.ReactNode;
}

const Layout = (props: LayoutProps) => {
  return (
    <>
      <DashboardNavBar>{props.children}</DashboardNavBar>
    </>
  );
};

export default Layout;
