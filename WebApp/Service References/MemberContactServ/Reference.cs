﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.MemberContactServ {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MemberContactServ.IMemberContactServ")]
    public interface IMemberContactServ {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberContactServ/GetList", ReplyAction="http://tempuri.org/IMemberContactServ/GetListResponse")]
        System.Collections.Generic.List<Model.MemberContact> GetList(string keyword, string type, int start, int size);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberContactServ/GetList", ReplyAction="http://tempuri.org/IMemberContactServ/GetListResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<Model.MemberContact>> GetListAsync(string keyword, string type, int start, int size);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberContactServ/GetCount", ReplyAction="http://tempuri.org/IMemberContactServ/GetCountResponse")]
        int GetCount(string keyword, string type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberContactServ/GetCount", ReplyAction="http://tempuri.org/IMemberContactServ/GetCountResponse")]
        System.Threading.Tasks.Task<int> GetCountAsync(string keyword, string type);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMemberContactServChannel : WebApp.MemberContactServ.IMemberContactServ, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MemberContactServClient : System.ServiceModel.ClientBase<WebApp.MemberContactServ.IMemberContactServ>, WebApp.MemberContactServ.IMemberContactServ {
        
        public MemberContactServClient() {
        }
        
        public MemberContactServClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MemberContactServClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MemberContactServClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MemberContactServClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<Model.MemberContact> GetList(string keyword, string type, int start, int size) {
            return base.Channel.GetList(keyword, type, start, size);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<Model.MemberContact>> GetListAsync(string keyword, string type, int start, int size) {
            return base.Channel.GetListAsync(keyword, type, start, size);
        }
        
        public int GetCount(string keyword, string type) {
            return base.Channel.GetCount(keyword, type);
        }
        
        public System.Threading.Tasks.Task<int> GetCountAsync(string keyword, string type) {
            return base.Channel.GetCountAsync(keyword, type);
        }
    }
}
