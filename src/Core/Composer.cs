using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JustCompose.Core
{
    public abstract class Composer
    {
        protected Composer(ComposerProperties properties)
        {
            Properties = properties;
        }

        protected ComposerProperties Properties { get; }

        public virtual Task<IComposerContext> UpAsync()
        {
            IComposerContext context = Up();
            return Task.FromResult(context);
        }

        protected virtual IComposerContext Up()
        {
            throw new NotImplementedException();
        }

        public virtual Task DownAsync(IComposerContext context)
        {
            Down(context);
            return Task.CompletedTask;
        }

        protected virtual void Down(IComposerContext context)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<PropertyDescriptor> GetPropertyDescriptors()
        {
            yield break;
        }
    }
}
