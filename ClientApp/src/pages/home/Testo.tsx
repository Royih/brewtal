import { useEffect } from "react";

export const Testo = () => {
  //const signalr = useContext(SignalrContext);

  useEffect(() => {
    console.log("Component reload [Testo]");
  });

  return <div>Hello World!</div>;
};
