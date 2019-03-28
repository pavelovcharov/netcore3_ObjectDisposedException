using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace netcore3_ObjectDisposedException {
    static class Program {
        [TestFixture]
        public class TestClass {
            [Test]
            public void ObjectDisposedEx() {
                ThreadExceptionEventArgs ea = null;
                Application.ThreadException += (s, e) => ea = e;
                Form form = new Form { Size = new Size(500, 400), };

                form.Show();
                form.Refresh();
                form.Show(); // second call to actually show form work in debug mode

                var control = new TextBox { Size = new Size(200, 200) };
                form.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                control.Focus();
                Assert.IsTrue(control.Focused);
                control.LostFocus += (s, e) => {
                    control.Dispose();
                    control = null;
                };
                form.Focus();
                Assert.IsTrue(form.Focused);

                if(ea != null)
                    Assert.Fail(ea.Exception.StackTrace);
            }
        }
    }
}
