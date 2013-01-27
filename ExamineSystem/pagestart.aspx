<%@ Page Title="" Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="pagestart.aspx.cs" Inherits="ExamineSystem.pagestart" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script language="javascript">
    parent.parent.HideShowDivFunc(true);
</script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<body topmargin="0" leftmargin="20">
<table cellpadding="0" cellspacing="1" border="0" class="TableStyle" align="center" style="width: 90%; margin-top:5px;">
	<tr><th class="HeadStyle" height="25">校园网络在线考试系统</th></tr>
	<tr>
		<td>
			<table border="0" cellpadding="0" cellspacing="0" bgcolor="#f1f3f5">
				<tr><td>
					<table border="0" cellpadding="0" cellspacing="0" width="95%" align="center">
						<tr><td height="35"><strong>系统简介</strong></td></tr>
						<tr><td>
							<ol>
								<li>校园网络在线考试系统，是完全针对英语等级考试进行的系统设计，其中包含五大类型题目（听力分析，阅读理解，单项选择，多项选择，对错判断）</li>
								<li>本系统全部采用客观题模式，试题考卷由系统随机筛选生成，并采用即时判分模式，系统中还包含历史分数查询功能可以随时观看原来的考试分数。</li>
                                <li>本系统的所有考题都是由教师权限的用户进行录入和维护，并可以针对每一个考题设定分数和难易度，还可以进行网络考场中的考试时间和类型题的出题量，及上传音频文件的大小设定工作。</li>
							</ol>
						</td></tr>
					</table>
				</td></tr>
			</table>
			<table border="0" cellPadding="0" cellSpacing="1" width="100%" align="center" bgcolor="#ffffff">
				<tr bgcolor="#f1f3f5" height="23">
				    <td width="15%" align="center">程序名称：</td>
				    <td width="85%">&nbsp;校园网络在线考试系统</td>
				</tr>
				<tr bgcolor="#f1f3f5" height="23">
					<td width="15%" align="center">版本序号：</td>
					<td width="85%">&nbsp;V 1.0</td>
				</tr>
				<tr bgcolor="#f1f3f5" height="23">
					<td width="15%" align="center">程序作者：</td>
					<td width="85%">&nbsp;浩良，ETAO</td>
				</tr>
				<tr bgcolor="#f1f3f5" height="23">	
					<td width="15%" align="center">程序介绍：</td>
					<td width="85%">&nbsp;校园网络在线考试系统是专为英语等级考试而设计</td>
				</tr>
				<tr bgcolor="#f1f3f5" height="23">
					<td width="15%" align="center">使 用 权：</td>
					<td width="85%">&nbsp;任何愿意使用本程序的用户</td>
				</tr>
				<tr bgcolor="#f1f3f5" height="23">
					<td width="15%" align="center">开发工具：</td>
					<td width="85%">&nbsp;ASP.NET + SQL2005</td>
				</tr>
				<tr bgcolor="#f1f3f5" height="23">
					<td width="15%" align="center">制作日期：</td>
					<td width="85%">&nbsp;2011年03月05日</td>
				</tr>
			</table>
		</td>
	</tr>
</table><br>

</body>

<script language="javascript">
    parent.parent.HideShowDivFunc(false);
</script>

</asp:Content>