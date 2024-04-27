import MainPageNavBar from "@/components/MainPageNavBar";

interface LayoutProps {
  children: React.ReactNode;
}

const Layout = (props: LayoutProps) => {
  return (
    <>
      <MainPageNavBar />
      {props.children}
    </>
  );
};

export default Layout;
