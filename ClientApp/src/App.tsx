import "./App.css";
import { ThemeContextProvider } from "./infrastructure/ThemeContextProvider";
import { CssBaseline } from "@material-ui/core";
import { SnackbarProvider } from "notistack";
import { Home } from "./pages/home/Home";
import { Route } from "react-router";
import { Footer } from "./pages/footer/Footer";
import { ApiContextProvider } from "./infrastructure/ApiContextProvider";
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
import { Layout } from "./pages/Layout";
import { Sessions } from "./pages/sessions/Sessions";

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
              <Route exact path="/sessions/:id?" component={Sessions} />
            </Layout>
            <Footer />
          </SnackbarProvider>
        </ThemeContextProvider>
      </SignalrContextProvider>
    </ApiContextProvider>
  );
}

export default App;
