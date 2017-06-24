using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using System.Reflection.Emit;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using System.Reflection;
using ZCompileKit;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class ExpVar : Exp, ISetter
    {
        public Token VarToken { get; set; }

        public string VarName { get { return VarToken.GetText(); } }
        public SymbolBase VarSymbol { get; set; }

        public void SetAssigned(ZType retType)
        {
            RetType = retType;
        }

        public Exp AnalyDim()
        {
            SymbolLocalVar localVarSymbol = new SymbolLocalVar(VarName, RetType);
            localVarSymbol.LoacalVarIndex = this.ExpContext.ProcContext.CreateLocalVarIndex(VarName);
            this.ProcContext.Symbols.Add(localVarSymbol);
            VarSymbol = localVarSymbol;

            WordInfo word = new WordInfo(VarName, WordKind.VarName,this);
            this.ExpContext.ProcContext.ProcVarWordDictionary.Add(word);
            return this;
        }

        public override Exp Analy( )
        {
            var symbols = this.ProcContext.Symbols; 
            VarSymbol = symbols.Get(VarName);
            if (VarSymbol == null)
            {
                ErrorE(this.Postion, "'{0}'不存在", VarName);
            }
            else
            {
                RetType = VarSymbol.SymbolZType;
            }
            return this;
        }

        SymbolDefField NestedFieldSymbol;
        public void SetAsLambdaFiled(SymbolDefField fieldSymbol)
        {
            NestedFieldSymbol = fieldSymbol;
        }

        #region Emit
        public override void Emit()
        {
            EmitGet();
        }

        public void EmitGet()
        {
            if (IsNested)
            {
                if(this.NestedFieldSymbol!=null)
                {
                    EmitHelper.Emit_LoadThis(IL);
                    EmitSymbolHelper.EmitLoad(IL,this.NestedFieldSymbol);
                    base.EmitConv();
                }
                else if (VarSymbol is SymbolDefProperty)
                {
                    if (this.ClassContext.IsStaticClass == false)
                    {
                        EmitHelper.Emit_LoadThis(IL);
                        EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
                    }
                    EmitSymbolHelper.EmitLoad(IL, VarSymbol);
                    base.EmitConv();
                }
                else
                {
                    throw new CompileException();
                }
            }
            else
            {
                if (EmitSymbolHelper.NeedCallThis(VarSymbol))
                {
                    EmitHelper.Emit_LoadThis(IL);
                }
                EmitSymbolHelper.EmitLoad(IL, VarSymbol);
                base.EmitConv();
            }
        }

        public void EmitSet( Exp valueExp)
        {
            if (IsNested)
            {
                if (this.NestedFieldSymbol != null)
                {
                    EmitHelper.Emit_LoadThis(IL);
                    EmitValueExp(valueExp);
                    EmitSymbolHelper.EmitStorm(IL, this.NestedFieldSymbol);
                }
                else if (VarSymbol is SymbolDefProperty)
                {
                    if (this.ClassContext.IsStaticClass == false)
                    {
                        EmitHelper.Emit_LoadThis(IL);
                        EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
                    }
                    EmitValueExp(valueExp);
                    EmitSymbolHelper.EmitStorm(IL, VarSymbol);
                }
                else
                {
                    throw new CompileException();
                }
            }
            else
            {
                if (EmitSymbolHelper.NeedCallThis(VarSymbol))
                {
                    EmitHelper.Emit_LoadThis(IL);
                }
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, VarSymbol);
                base.EmitConv();
            }
        }

        private void EmitValueExp(Exp valueExp)
        {
            valueExp.RequireType = this.RetType;
            valueExp.Emit();
        }

        #endregion


        #region 覆盖

        public bool CanWrite
        {
            get
            {
                return VarSymbol.CanWrite;
            }
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        public override string ToString()
        {
            return VarToken.GetText();
        }

        public override CodePosition Postion
        {
            get
            {
                return VarToken.Position;
            }
        }
        #endregion
    }
}
