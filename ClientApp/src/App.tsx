import React from "react";
import "./App.css";
import { ThemeContextProvider } from "./infrastructure/ThemeContextProvider";
import { CssBaseline } from "@material-ui/core";
import { Counter } from "./components/functionDemo/Counter";
import { SnackbarProvider } from "notistack";
import { Layout } from "./components/Layout";
import { Home } from "./components/home/Home";
import { Route } from "react-router";
import { Footer } from "./components/footer/Footer";
import { ApiContextProvider } from "./infrastructure/ApiContextProvider";
import { FetchData } from "./components/functionDemo/FetchData";
import { ThrowExceptions } from "./components/functionDemo/TestExceptions";
import { SignalrContextProvider } from "./infrastructure/SignalrContextProvider";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faBug,
  faCaretSquareDown,
  faChevronCircleRight,
  faClipboardList,
  faCog,
  faHeartbeat,
  faHome,
  faLightbulb,
  faMehBlank,
  faQuestionCircle,
  fas,
  faSatelliteDish,
  faSun,
  faTemperatureLow,
  faThermometerEmpty,
  faToggleOff,
  faToggleOn,
  faTrash,
  faUserLock,
  faUsers,
  faVolumeDown,
  faVolumeUp,
  faSyncAlt,
  faHeartBroken,
  faUnlink,
} from "@fortawesome/free-solid-svg-icons";

library.add(
  fas,
  faTemperatureLow,
  faSatelliteDish,
  faClipboardList,
  faHome,
  faUsers,
  faChevronCircleRight,
  faVolumeUp,
  faVolumeDown,
  faThermometerEmpty,
  faSun,
  faToggleOn,
  faToggleOff,
  faLightbulb,
  faQuestionCircle,
  faMehBlank,
  faBug,
  faTrash,
  faCog,
  faUserLock,
  faHeartbeat,
  faCaretSquareDown,
  faSyncAlt,
  faHeartbeat,
  faHeartBroken,
  faUnlink
);

function App() {
  return (
    <ApiContextProvider>
      <SignalrContextProvider>
        <ThemeContextProvider>
          <CssBaseline />
          <SnackbarProvider maxSnack={10}>
            <Layout>
              <Route exact path="/" component={Home} />
              <Route exact path="/counter" component={Counter} />
              <Route exact path="/fetch-data" component={FetchData} />
              <Route exact path="/throw-exceptions" component={ThrowExceptions} />
            </Layout>
            <Footer />
          </SnackbarProvider>
        </ThemeContextProvider>
      </SignalrContextProvider>
    </ApiContextProvider>
  );
}

export default App;
