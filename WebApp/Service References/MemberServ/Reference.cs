﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.MemberServ {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MemberServ.IMemberServ")]
    public interface IMemberServ {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberServ/GetList", ReplyAction="http://tempuri.org/IMemberServ/GetListResponse")]
        System.Collections.Generic.List<Model.Member> GetList(string keyword, string type, int start, int size);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberServ/GetList", ReplyAction="http://tempuri.org/IMemberServ/GetListResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<Model.Member>> GetListAsync(string keyword, string type, int start, int size);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberServ/GetCount", ReplyAction="http://tempuri.org/IMemberServ/GetCountResponse")]
        int GetCount(string keyword, string type);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMemberServ/GetCount", ReplyAction="http://tempuri.org/IMemberServ/GetCountResponse")]
        System.Threading.Tasks.Task<int> GetCountAsync(string keyword, string type);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMemberServChannel : WebApp.MemberServ.IMemberServ, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MemberServClient : System.ServiceModel.ClientBase<WebApp.MemberServ.IMemberServ>, WebApp.MemberServ.IMemberServ {
        
        public MemberServClient() {
        }
        
        public MemberServClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MemberServClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MemberServClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MemberServClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<Model.Member> GetList(string keyword, string type, int start, int size) {
            return base.Channel.GetList(keyword, type, start, size);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<Model.Member>> GetListAsync(string keyword, string type, int start, int size) {
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