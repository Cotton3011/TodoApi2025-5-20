namespace TodoApi.Dtos
{
	//フロントエンドから送られてくるデータを受け取る箱
	public class CreateTodoDto
	{
		//入力されたTODOタイトル
		public string Title { get; set; } = string.Empty;
	}
}
