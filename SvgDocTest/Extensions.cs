using System;
using System.Windows.Forms;

namespace SvgDocTest {
    public static class Extensions {
        public static void RefreshFrom(this WebBrowser browser, string filename) {
            browser.Navigate(new Uri(filename));
            browser.Refresh(WebBrowserRefreshOption.Completely);
        }
    }
}
